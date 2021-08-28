using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace FFPark.Model
{
    public class BaseFFParkModel:BaseFFParkEntityModel
    {

        public BaseFFParkModel()
        {         
            PostInitialize();
        }

        public virtual void BindModel(ModelBindingContext bindingContext) { }

        protected virtual void PostInitialize() { }


      



    }
}
