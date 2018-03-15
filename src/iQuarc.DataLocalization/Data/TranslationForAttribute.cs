using System;

namespace iQuarc.DataLocalization
{
    [AttributeUsageAttribute(AttributeTargets.Class)]
    public class TranslationForAttribute : Attribute
    {
        public TranslationForAttribute(Type translatedEntity)
        {
            this.TranslatedEntity = translatedEntity;
        }

        public Type TranslatedEntity { get; }
    }
}