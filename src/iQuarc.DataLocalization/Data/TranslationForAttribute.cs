using System;

namespace iQuarc.DataLocalization
{
    public class TranslationForAttribute : Attribute
    {
        public TranslationForAttribute(Type translatedEntity)
        {
            this.TranslatedEntity = translatedEntity;
        }

        public Type TranslatedEntity { get; }
    }
}