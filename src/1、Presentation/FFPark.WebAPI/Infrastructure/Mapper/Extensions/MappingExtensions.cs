using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FFPark.Core;
using FFPark.Core.Infrastructure.Mapper;
using FFPark.Model;

namespace FFPark.WebAPI.Infrastructure.Mapper.Extensions
{
    public static class MappingExtensions
    {
        private static TDestination Map<TDestination>(this object source)
        {
            return AutoMapperConfiguration.Mapper.Map<TDestination>(source);
        }

        private static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            //use AutoMapper for mapping objects
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

        /// <summary>
        /// 实体转Model
        /// </summary>
        /// <typeparam name="TModel">Model Type</typeparam>
        /// <param name="entity">Entity to map from</param>
        /// <returns>Mapped Model</returns>
        public static TModel ToModel<TModel>(this BaseEntity entity) where TModel : BaseFFParkEntityModel
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return entity.Map<TModel>();
        }
        /// <summary>
        /// Execute a mapping from the entity to the existing model
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <typeparam name="TModel">Model type</typeparam>
        /// <param name="entity">Entity to map from</param>
        /// <param name="model">Model to map into</param>
        /// <returns>Mapped model</returns>
        public static TModel ToModel<TEntity, TModel>(this TEntity entity, TModel model)
            where TEntity : BaseEntity where TModel : BaseFFParkEntityModel
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return entity.MapTo(model);
        }
        public static TEntity ToEntity<TEntity>(this BaseFFParkEntityModel model) where TEntity : BaseEntity
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return model.Map<TEntity>();
        }

        public static TEntity ToEntity<TEntity, TModel>(this TModel model, TEntity entity)
          where TEntity : BaseEntity where TModel : BaseFFParkEntityModel
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return model.MapTo(entity);
        }
        /// <summary>
        /// 实体转Model/Dto
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<TModel> ToModelList<T, TModel>(this List<T> list) where TModel : BaseFFParkEntityModel
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            List<TModel> models = new List<TModel>();
            foreach (var entity in list)
            {
                var item = entity.Map<TModel>();
                models.Add(item);
            }
            return models;
        }
        /// <summary>
        /// Dto/Model-->Entity
        /// </summary>
        /// <typeparam name="TModel "></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<TEntity> ToEntityList<TModel, TEntity>(this List<TModel> list) where TEntity : BaseEntity
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            List<TEntity> listE = new List<TEntity>();
            foreach (var item in list)
            {
                var entity = item.Map<TEntity>();
                listE.Add(entity);
            }
            return listE;
        }

    }
}
