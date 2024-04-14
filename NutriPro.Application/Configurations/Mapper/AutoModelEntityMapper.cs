﻿//using AutoMapper;
//using NutriPro.Application.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web;
//using System.Xml.Serialization;

//namespace NutriPro.Application.Configurations.Mapper
//{
//    public class AutoModelEntityMapper<TEntity, TModel> : IModelEntityMapper<TEntity, TModel>
//            where TEntity : class, IBaseEntity
//            where TModel : ICrudModel
//    {
//        private readonly IRepository<TEntity> repository;

//        public AutoModelEntityMapper(IRepository<TEntity> repository)
//        {
//            this.repository = repository;
//        }

//        public TModel BuildModel(TEntity entity)
//        {
//            TModel model;

//            if (typeof(TModel).IsInterface)
//            {
//                model = (TModel)Engine.Resolve(typeof(TModel));
//            }
//            else
//            {
//                model = Activator.CreateInstance<TModel>();
//            }

//            if (!typeof(TEntity).IsAbstract)
//            {
//                Singleton<IMapper>.Instance.Map(entity, model);
//            }
//            else
//            {
//                Singleton<IMapper>.Instance.Map(entity, model, typeof(TEntity), typeof(TModel));
//            }

//            MakeModel(entity, ref model);

//            if (entity != null && entity.Id != 0)
//            {
//                model.SerializedOldValue = HttpUtility.HtmlEncode(model.SerializeObject());
//            }

//            return model;
//        }

//        protected virtual void MakeModel(TEntity entity, ref TModel model)
//        {
//        }

//        public TEntity BuildEntity(TModel model, long? id)
//        {
//            TEntity entity;

//            if (id.HasValue && id > 0)
//            {
//                entity = repository.GetById(id.Value);
//            }
//            else
//            {
//                if (typeof(TEntity).IsInterface)
//                {
//                    entity = (TEntity)Engine.Resolve(typeof(TEntity));
//                }
//                else if (!typeof(TEntity).IsAbstract)
//                {
//                    entity = Activator.CreateInstance<TEntity>();
//                }
//                else
//                {
//                    var concretEntityType = Singleton<IMapper>.Instance.Map(model, typeof(TModel), typeof(TEntity)).GetType();

//                    entity = (TEntity)Activator.CreateInstance(concretEntityType);
//                }

//            }

//            if (entity == null) throw new BusinessException("A entidade especificada não foi encontrada na base de dados. Por favor atualize a tela.");

//            var modelClone = model;

//            //substituir todas as propriedades sem nenhuma alteração pelo valor do banco no model
//            if (!string.IsNullOrEmpty(model.SerializedOldValue))
//            {
//                var xmlSerializer = new XmlSerializer(model.GetType());

//                using (var sr = new StringReader(HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(model.SerializedOldValue))))
//                {
//                    var oldModel = (TModel)xmlSerializer.Deserialize(sr);

//                    TModel modelFromEntity;

//                    if (typeof(TModel).IsInterface)
//                    {
//                        modelFromEntity = (TModel)Engine.Resolve(typeof(TModel));
//                    }
//                    else
//                    {
//                        modelFromEntity = Activator.CreateInstance<TModel>();
//                    }

//                    //geração de um modelo a partir da entidade para copiar as propriedades não modificadas
//                    Singleton<IMapper>.Instance.Map(entity, modelFromEntity);

//                    foreach (var pi in model.GetType().GetProperties())
//                    {
//                        //verifica apenas as propriedades que tenham o método set definido
//                        if (pi.SetMethod != null)
//                        {
//                            //neste ponto é feita uma procurar por todas as propriedades não alteradas do modelo
//                            if (pi.GetValue(model) == pi.GetValue(oldModel) || pi.GetValue(model) != null && pi.GetValue(model).Equals(pi.GetValue(oldModel)))
//                            {
//                                //para cada propriedade, copia o valor que veio do banco, assim não haverá update
//                                pi.SetValue(model, pi.GetValue(modelFromEntity));
//                            }
//                        }
//                    }
//                }
//            }

//            Singleton<IMapper>.Instance.Map(model, entity);

//            MakeEntity(ref entity, model);

//            return entity;
//        }

//        protected virtual void MakeEntity(ref TEntity e, TModel model)
//        {
//        }
//    }
//}
