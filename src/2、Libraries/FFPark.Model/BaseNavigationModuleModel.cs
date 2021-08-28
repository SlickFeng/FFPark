using System.ComponentModel.DataAnnotations;
namespace FFPark.Model
{
    public class BaseNavigationModuleModel : BaseFFParkModel
    {
        /// <summary>
        /// Title
        /// </summary>
        [Required(ErrorMessage = "Title不可为空")]
        public string Title { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [Required(ErrorMessage = "Name不可为空")]

        public string Name { get; set; }


        public int ParentId { get; set; }


        public string Icon { get;set;}


        public string TargetUrl { get;set;}


        public string Component { get;set;}

    }
}
