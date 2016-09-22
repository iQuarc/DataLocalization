iQuarc.DataLocalization
==========
Data Localization is a helper library for Linq based data access frameworks that allows easy writing queries for retrieving localized data.

Overview
-----------

The library provides a set of helper methods for querying localized data split in multiple tables. The library works by rewriting Linq that perform projections on the main table to retrieve data from localization tables when available.

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
	[LanugageID] int NOT NULL FOREIGN KEY REFERENCES [Languages]([ID]),
	[CategoryID] int NOT NULL FOREIGN KEY REFERENCES [Categories]([ID]),
	[Name] nvarchar(512) NOT NULL
)
```

The the Linq query to a list of categories without localized data would look like:

```csharp
var categories = ctx.Categories
		.Select(c => new
		{
			ID = c.ID,
			Name = c.Name
		}).ToList();
```

However to retrieve localized catetory names, for example the Frech version the query would need to look something like:

```csharp
var categories = ctx.Categories
		.Select(c => new 
		{ 
			ID = c.ID, 
			Name = c.CategoryTranslations.Where(ct => ct.Lanugage.IsoCode == "fr").Select(ct => ct.Name).FirstOrDefault() ?? c.Name
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

In the above query the `.Localize(...)` method call will automatically replace `Name = c.Name` in the projection with `Name = c.CategoryTranslations.Where(ct => ct.Lanugage.IsoCode == "fr").Select(ct => ct.Name).FirstOrDefault() ?? c.Name`
as if the localized query was written manually.


Getting Started
-----------------

1. First step is to register a localization entity, tipically this is done on the DbContext static constructor

```csharp
static AppDbContext()
{
	iQuarc.DataLocalization.LocalizationConfig.RegisterLocalizationEntity<Language>(l => l.IsoCode);
}

```

2. Mark all entities that contain localized data with `TranslationFor` attribute

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

3. Use the `.Localize()` extension method to localize queries containing projections (contain `.Select()` calls that [project data to new entities)

__! Important !__
========
The translation of Linq queries is **only** performed if there actually is a `linq projection`, otherwise the query is unaffected.

Related Repositories
-------------------------
[iQuarc.AppBoot](https://github.com/iQuarc/AppBoot)
[iQuarc.DataAccess](https://github.com/iQuarc/DataAccess)
