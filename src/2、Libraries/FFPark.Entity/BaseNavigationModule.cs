using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using FFPark.Core;
namespace FFPark.Entity
{
    /// <summary>
    /// 左侧导航实体
    /// </summary>
    [Table(Name = "NavigationModule")]
    public class BaseNavigationModule : BaseEntity
    {
        [JsonProperty, Column(Name = "Id", IsPrimary = true)]
        public override int Id { get => base.Id; set => base.Id = value; }

        public string Title { get; set; }

        public string Name { get; set; }

        public string Component { get; set; }


        public string TargetUrl { get; set; }

        [JsonProperty, Navigate("ParentId")]
        public int ParentId { get; set; }

        public string Icon { get; set; }

        public List<BaseNavigationModule> Child { get;set;}
        
    }
}
