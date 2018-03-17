iQuarc.DataLocalization
==========
Data Localization is a helper library for Linq based data access frameworks (Such as Entity Framework 6, Entity Framework Core, Linq2SQL, nHibernate, and others) that allows easy writing queries for retrieving localized data.

Overview
-----------

The library provides a set of helper methods for querying localized data split in multiple tables. The library works by rewriting Linq expression trees that perform projections on the main table, to join and retrieve data from localization tables when available.

Tutorial
-----------------

For a detailed step by step tororial **Daniel Kvis** has written a 4 part blog series: 

 - [Simple data localisation with .NET core – Part 1 – Set up basic .NET core MVC project](https://danielkvist.net/code/simple-data-localisation-with-net-core-and-iquarc-datalocalization-from-scratch-part-1-set-up-basic-net-core-mvc-project)
 - [Simple data localisation with .NET core – Part 2 – Add models, database and migrations with code first](https://danielkvist.net/code/simple-data-localisation-with-net-core-and-iquarc-datalocalization-from-scratch-part-2-add-models-database-and-migrations-with-code-first)
 - [Simple data localisation with .NET core – Part 3 – Add API Controller to view model data](https://danielkvist.net/code/simple-data-localisation-with-net-core-and-iquarc-datalocalization-from-scratch-part-3-add-api-controller-to-view-model-data)
 - [Simple data localisation with .NET core – Part 4 – Add data localisation for models using iQuarc.DataLocalization](https://danielkvist.net/code/simple-data-localisation-with-net-core-and-iquarc-datalocalization-from-scratch-part-4-add-data-localisation-for-models-using-iquarc-datalocalization)
 

Usage Patterns
------------------

For example let's consider the following database which contains a `Categories` table and a `CategoryLocalization` table with localized data for categories: 

```sql
CREATE TABLE [Languages]
(
	[ID] int  NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Name] nvarchar(255) NOT NULL,
	[IsoCode] nchar(2) NOT NULL
)

CREATE TABLE [Categories]
(
	[ID] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Name] nvarchar(512) NOT NULL
)

CREATE TABLE [CategoryTranslations]
(
	[ID] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[LanguageID] int NOT NULL FOREIGN KEY REFERENCES [Languages]([ID]),
	[CategoryID] int NOT NULL FOREIGN KEY REFERENCES [Categories]([ID]),
	[Name] nvarchar(512) NOT NULL
)
```

The Linq query to a list of categories without localized data would look like:

```csharp
var categories = ctx.Categories
		.Select(c => new
		{
			ID = c.ID,
			Name = c.Name
		}).ToList();
```

However to retrieve localized category names, for example the French version the query would need to look something like:

```csharp
var categories = ctx.Categories
		.Select(c => new 
		{ 
			ID = c.ID, 
			Name = c.CategoryTranslations.Where(ct => ct.Language.IsoCode == "fr").Select(ct => ct.Name).FirstOrDefault() ?? c.Name
		}).ToList();
```

This pattern of writing queries becomes very tedious and error prone in time. This is where **iQuarc.DataLocalization** comes in, a simple set of helper methods that enable simple Linq queries to be localized automatically.

Using **iQuarc.DataLocalization**

```csharp
var categories = this.Categories
		.Select(c => new
		{
			ID = c.ID,
			Name = c.Name
		})
		.Localize(new CultureInfo("fr-Fr")
		.ToList();
```

In the above query the `.Localize(...)` method call will automatically replace `Name = c.Name` in the projection with `Name = c.CategoryTranslations.Where(ct => ct.Language.IsoCode == "fr").Select(ct => ct.Name).FirstOrDefault() ?? c.Name`
as if the localized query was written manually.


Getting Started
-----------------
***Step 1.***  Install from Nuget: https://www.nuget.org/packages/iQuarc.DataLocalization/

Or from Command Line:
	`PM> Install-Package iQuarc.DataLocalization`

***Step 2.*** First step is to register a localization entity, typically this is done on the DbContext static constructor

```csharp
static AppDbContext()
{
	iQuarc.DataLocalization.LocalizationConfig.RegisterLocalizationEntity<Language>(l => l.IsoCode);
}

```

***Step 3.*** Mark all entities that contain localized data with `[TranslationFor]` attribute

```csharp
[TranslationFor(typeof(Category))]
public class CategoryTranslation
{
	public int ID {get;set;}
	public int LanguageID {get;set;}
	public int CategoryID {get;set;}
	public string Name {get;set;}
	...
}
```

***Step 4.*** Use the `.Localize()` extension method to localize queries containing projections (contain `.Select()` calls that [project data to new entities)

__! Important !__
========
The translation of Linq queries is **only** performed if there actually is a `linq projection`, otherwise the query is unaffected.

Related Repositories
-------------------------
[iQuarc.AppBoot](https://github.com/iQuarc/AppBoot)
[iQuarc.DataAccess](https://github.com/iQuarc/DataAccess)


Changing the localization entity identifying key
---------------------------------------------------

In order to change the key used to identify the localization entity the following configurations can be used:

```csharp
        LocalizationConfig.RegisterLocalizationEntity<Language>(l => l.LCID);
        LocalizationConfig.RegisterCultureMapper(c => c.LCID);
```

The example sets up the LCID to be the language idetifier, which is a numeric code part of [[RFC5646]](http://www.rfc-editor.org/rfc/bcp/bcp47.txt) 
[See Also, Microsoft LCID structure:](https://msdn.microsoft.com/en-us/library/cc233968.aspx?f=255&MSPPError=-2147217396)
