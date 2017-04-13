using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace RogueFlashNetCoreMvc.Support
{
    public class EmptyStringDisplayMetadataProvider : IDisplayMetadataProvider
    {
        /*
         * Bug in AspNetCore.Mvc previous to version 1.1.0:
         * 
         *  https://github.com/aspnet/Mvc/issues/5086
         *  
         *  Use an implementation of IModelBinder as suggested:

            public class ModelBinderExample : IModelBinder
            {
                public Task BindModelAsync(ModelBindingContext context)
                {
                    // See SimpleTypeModelBinder in Microsoft.AspNetCore.Mvc.ModelBinding.Binders:
                    //  https://github.com/aspnet/Mvc/blob/760c8f38678118734399c58c2dac981ea6e47046/src/Microsoft.AspNetCore.Mvc.Core/ModelBinding/Binders/SimpleTypeModelBinder.cs
                }
            }

            public class ModelBinderExampleProvider : IModelBinderProvider
            {
                public IModelBinder GetBinder(ModelBinderProviderContext context)
                {
                    // See SimpleTypeModelBinderProvider in Microsoft.AspNetCore.Mvc.ModelBinding.Binders:
                    //  https://github.com/aspnet/Mvc/blob/760c8f38678118734399c58c2dac981ea6e47046/src/Microsoft.AspNetCore.Mvc.Core/ModelBinding/Binders/SimpleTypeModelBinderProvider.cs

                    // Then:
                    //  services.AddMvc().AddMvcOptions(options => options.ModelBinderProviders.Add(new ModelBinderExampleProvider()));
                    // See:
                    //  https://github.com/aspnet/Mvc/blob/927e75870d53fa1d21df21995fc5a05f318eccdc/src/Microsoft.AspNetCore.Mvc.Core/Internal/MvcCoreMvcOptionsSetup.cs
                }
            }
            
        *   From version 1.1.0, it is possible to use an implementation of IDisplayMetadataProvider:
        *   
        *   https://github.com/aspnet/Mvc/issues/4988
        * 
        */

        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            if (context.Key.MetadataKind != ModelMetadataKind.Property)
            {
                return;
            }
            if (context.Key.ModelType != typeof(string))
            {
                return;
            }
            context.DisplayMetadata.ConvertEmptyStringToNull = false;
        }
    }
}
