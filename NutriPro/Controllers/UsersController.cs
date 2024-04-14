using AutoMapper;
using NutriPro.Application;
using NutriPro.Application.Configurations;
using NutriPro.Application.Dtos.Management;
using NutriPro.Application.Interfaces.Management;
using NutriPro.Data.Models.Management;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace NutriPro.Mvc.Controllers
{
    public class UsersController : NutriProControllerBase<Users, UsersDto, long>
    {
        private readonly IUnitsAppService _unitsAppServices;
        private readonly IUsersAppService _usersAppServices;

        public UsersController(NutriProSession session, IMapper mapper, IUnitsAppService UnitsAppService, IUsersAppService usersAppServices, NutriProAppServiceBase<Users> userService) : base(session, mapper, userService)
        {
            _unitsAppServices = UnitsAppService;
            _usersAppServices = usersAppServices;
        }

        #region OVERRIDE USERS

        public override async Task<ActionResult> EditModal(long id)
        {
            var entity = await _appService.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<UsersDto>(entity);
            dto.DecryptPassword();
            return PartialView("_EditModal", dto);
        }

        [HttpPost]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public override async Task<IActionResult> InsertAsync(UsersDto model)
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
                var entity = _mapper.Map<Users>(model);

                if (!string.IsNullOrEmpty(model.SerializedUnitsList))
                {
                    var unitsId = JsonConvert.DeserializeObject<List<long>>(model.SerializedUnitsList);
                    if (unitsId?.Count > 0)
                    {
                        var units = await _unitsAppServices.GetByListIdsAsync(unitsId);
                        if (units?.Any() == true)
                        {
                            entity.Units.AddRange(units);
                        }
                    }
                }

                var encryptionService = new AesEncryptionService();

                entity.Password = encryptionService.Encrypt(entity.Password);
                await _usersAppServices.CreateAsync(entity);

                var response = new
                {
                    redirectTo = Url.Action("Index", "Users")
                };

                return Ok(response);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar os dados! Contate o suporte.");
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public override async Task<IActionResult> UpdateAsync(long id, UsersDto model)
        {
            try
            {
                var entity = await _appService.GetByIdAsync(id);
                if (entity == null)
                {
                    return NotFound();
                }


                var entityProperties = typeof(Users).GetProperties();
                var modelProperties = typeof(UsersDto).GetProperties();

                foreach (var modelProperty in modelProperties)
                {
                    if (modelProperty.Name != "Id")
                    {
                        var modelValue = modelProperty.GetValue(model);
                        var entityProperty = entityProperties.FirstOrDefault(p => p.Name == modelProperty.Name);

                        if (entityProperty != null)
                        {
                            var entityValue = entityProperty.GetValue(entity);

                            if (modelValue != null && entityValue != null && !modelValue.Equals(entityValue))
                            {
                                entityProperty.SetValue(entity, modelValue);
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(model.SerializedUnitsList))
                {
                    var unitsId = JsonConvert.DeserializeObject<List<long>>(model.SerializedUnitsList);
                    if (unitsId?.Count > 0)
                    {
                        var units = await _unitsAppServices.GetByListIdsAsync(unitsId);
                        if (units?.Any() == true)
                        {
                            entity.Units.Clear();
                            entity.Units.AddRange(units);
                        }
                    }
                }

                var encryptionService = new AesEncryptionService();

                entity.Password = encryptionService.Encrypt(entity.Password);

                await _usersAppServices.UpdateAsync(entity);

                return Ok();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao excluir os dados! Contate o suporte.");
                return BadRequest(ModelState);
            }
        } 
        #endregion
    }
}
