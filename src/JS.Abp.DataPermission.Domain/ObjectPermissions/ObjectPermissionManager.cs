using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace JS.Abp.DataPermission.ObjectPermissions
{
    public class ObjectPermissionManager : DomainService
    {
        private readonly IObjectPermissionRepository _objectPermissionRepository;

        public ObjectPermissionManager(IObjectPermissionRepository objectPermissionRepository)
        {
            _objectPermissionRepository = objectPermissionRepository;
        }

        public async Task<ObjectPermission> CreateAsync(
         string objectName, string description)
        {
            Check.NotNullOrWhiteSpace(objectName, nameof(objectName));
            Check.Length(objectName, nameof(objectName), ObjectPermissionConsts.ObjectNameMaxLength);


            var objectPermission = new ObjectPermission(
             GuidGenerator.Create(),
             objectName, description
             );

            return await _objectPermissionRepository.InsertAsync(objectPermission);
        }

        public async Task<ObjectPermission> UpdateAsync(
            Guid id,
            string objectName, string description, [CanBeNull] string concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(objectName, nameof(objectName));
            Check.Length(objectName, nameof(objectName), ObjectPermissionConsts.ObjectNameMaxLength);


            var objectPermission = await _objectPermissionRepository.GetAsync(id);

            objectPermission.Description = description;

            objectPermission.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _objectPermissionRepository.UpdateAsync(objectPermission);
        }

    }
}