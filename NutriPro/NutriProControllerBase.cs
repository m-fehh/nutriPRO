using AutoMapper;
using NutriPro.Application;
using NutriPro.Application.Configurations;
using NutriPro.Application.Dtos.Management.Tenants;
using NutriPro.Data.Models.Management;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace NutriPro.Mvc
{
    public class NutriProControllerBase<TEntity, TDto, TEntityId> : Controller where TEntity : class where TDto : class, new()
    {
        private readonly NutriProSession _session;
        protected readonly IMapper _mapper;
        protected readonly NutriProAppServiceBase<TEntity> _appService;

        public NutriProControllerBase(NutriProSession session, IMapper mapper, NutriProAppServiceBase<TEntity> appService)
        {
            _session = session;
            _mapper = mapper;
            _appService = appService;
        }

        protected void SetViewBagValues()
        {
            ViewBag.TenantId = _session.TenantId;
            ViewBag.UserName = _session.UserName?.ToString().ToUpper();
            ViewBag.IsHost = IsInHostMode();
        }

        protected bool IsInHostMode()
        {
            return _session.IsInHostMode();
        }


        #region CRUD DEFAULT 
        [HttpGet]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public virtual async Task<IActionResult> GetByIdAsync(TEntityId id)
        {
            try
            {
                var entity = await _appService.GetByIdAsync<TEntityId>(id);
                if (entity == null)
                {
                    return NotFound();
                }

                var dto = _mapper.Map<TDto>(entity);

                return Ok(dto);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao buscar os dados! Contate o suporte.");
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public virtual async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var entities = await _appService.GetAllAsync();
                var dtos = _mapper.Map<List<TDto>>(entities);

                return Ok(dtos);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao buscar os dados! Contate o suporte.");
                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public virtual async Task<IActionResult> GetAllPaginated([FromBody] UserDataTableParams dataTableParams)
        {
            if (dataTableParams == null)
            {
                return BadRequest("Parâmetros inválidos.");
            }

            var tenantId = RequiresTenantOrganizationFilter<TDto>() ? _session.TenantId ?? null : null;
            var listUnitsId = RequiresTenantOrganizationFilter<TDto>() ? _session.OrganizationId : null;

            var result = await _appService.GetAllPaginatedAsync(dataTableParams, tenantId, listUnitsId, _session.IsHost);

            var dtos = _mapper.Map<List<TDto>>(result.Entities);

            var data = new
            {
                draw = dataTableParams.Draw,
                recordsTotal = result.TotalRecords,
                recordsFiltered = result.TotalRecords,
                data = dtos
            };

            // Criar as configurações de serialização
            var serializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            // Serializar o objeto usando as configurações criadas
            var json = JsonConvert.SerializeObject(data, serializerSettings);

            return Content(json, "application/json");
        }

        [HttpPost]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public virtual async Task<IActionResult> InsertAsync(TDto model)
        {
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    ModelState.AddModelError(validationResult.MemberNames.FirstOrDefault() ?? string.Empty, validationResult.ErrorMessage);
                }

                return BadRequest(ModelState);
            }

            try
            {
                //// Verificar se os dados já existem antes de inserir
                //var exists = await EntityExistsAsync(e => IsDuplicate(e, model));
                //if (exists)
                //{
                //    ModelState.AddModelError(string.Empty, "Os dados já existem no banco de dados.");
                //    return BadRequest(ModelState);
                //}


                var entity = _mapper.Map<TEntity>(model);

                var tenantIdProperty = typeof(TDto).GetProperty("TenantId");
                if (tenantIdProperty != null)
                {
                    var tenantIdValue = tenantIdProperty.GetValue(model);

                    if (tenantIdValue == null)
                    {
                        long tenantId = _session.TenantId.GetValueOrDefault();
                        typeof(TEntity).GetProperty("TenantId")?.SetValue(entity, tenantId);
                    }
                    else
                    {
                        typeof(TEntity).GetProperty("TenantId")?.SetValue(entity, tenantIdValue);
                    }
                }

                //var organizationIdProperty = typeof(TDto).GetProperty("OrganizationId");
                //if (organizationIdProperty != null)
                //{
                //    var organizationIdValue = organizationIdProperty.GetValue(model);
                //    if (organizationIdValue == null)
                //    {
                //        long organizationId = _session.OrganizationId.GetValueOrDefault();
                //        typeof(TEntity).GetProperty("OrganizationId")?.SetValue(entity, organizationId);
                //    }
                //    else
                //    {
                //        typeof(TEntity).GetProperty("OrganizationId")?.SetValue(entity, organizationIdValue);
                //    }
                //}

                await _appService.CreateAsync(entity);

                var controllerName = typeof(TEntity).Name.Replace("Entity", string.Empty);
                var response = new
                {
                    redirectTo = Url.Action("Index", controllerName)
                };

                return Ok(response);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar os dados! Contate o suporte.");
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public virtual async Task<IActionResult> DeleteAsync(TEntityId id)
        {
            try
            {
                var entity = await _appService.GetByIdAsync(id);
                if (entity == null)
                {
                    return NotFound();
                }

                var isDeletedProperty = typeof(TEntity).GetProperty("IsDeleted");
                if (isDeletedProperty != null)
                {
                    isDeletedProperty.SetValue(entity, true);
                    await _appService.UpdateAsync(entity);
                }
                else
                {
                    await _appService.DeleteAsync(entity);
                }

                return Ok();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao excluir os dados! Contate o suporte.");
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public virtual async Task<IActionResult> UpdateAsync(TEntityId id, TDto model)
        {
            try
            {
                var entity = await _appService.GetByIdAsync(id);
                if (entity == null)
                {
                    return NotFound();
                }

                // Comparar propriedades do model com a entity
                foreach (var property in typeof(TDto).GetProperties())
                {
                    if (property.Name != "Id")
                    {
                        var modelValue = property.GetValue(model);
                        var entityProperty = typeof(TEntity).GetProperty(property.Name);

                        if (modelValue != null && entityProperty != null)
                        {
                            // Verifica se a propriedade é um enum
                            if (entityProperty.PropertyType.IsEnum && modelValue is string)
                            {
                                var enumValue = Enum.Parse(entityProperty.PropertyType, (string)modelValue);
                                entityProperty.SetValue(entity, enumValue);
                            }
                            else
                            {
                                // Se não for um enum, faz a atribuição direta
                                entityProperty.SetValue(entity, modelValue);
                            }
                        }
                    }
                }


                await _appService.UpdateAsync(entity);

                return Ok();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao excluir os dados! Contate o suporte.");
                return BadRequest(ModelState);
            }
        }

        #endregion

        #region VIEWS

        public virtual IActionResult Index()
        {
            SetViewBagValues();
            return View();
        }

        public virtual IActionResult Create()
        {
            SetViewBagValues();
            return View(new TDto());
        }

        public virtual async Task<ActionResult> EditModal(TEntityId id)
        {
            var entity = await _appService.GetByIdAsync<TEntityId>(id);
            if (entity == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<TDto>(entity);
            return PartialView("_EditModal", dto);
        }

        #endregion

        #region PRIVATE METHODS 

        private bool RequiresTenantOrganizationFilter<T>()
        {
            var classesWithoutFilter = new List<Type> {
                typeof(TenantsDto)
            };

            var result = !classesWithoutFilter.Contains(typeof(T));
            return result;
        }

        #endregion
    }
}
