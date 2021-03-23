using System;
using System.Threading.Tasks;

using Dapper;
using DNSeed.Domain;

namespace DNSeed.Repositories
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly IDalSession _session;

        public UserRepository(IDalSession session)
        {
            _session = session;
        }

        public async Task<LoginResponse> SigininAsync(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username))
                request.Username = "";
            if (string.IsNullOrEmpty(request.Password))
                request.Password = "";

            var readConnection = await _session.GetReadOnlyConnectionAsync();
            string sql = @"SELECT TOP 1 [Id],[LoginName] FROM dbo.Authentication WHERE [LoginName]=@username AND [Password]=@password;";
            LoginResponse response = await readConnection.QueryFirstOrDefaultAsync<LoginResponse>(sql, new { 
                username = request.Username, 
                password = request.Password 
            });

            if(null != response)
            {
                var uom = _session.GetUnitOfWork();
                var writeConnection = await uom.GetConnectionAsync();
                sql = @"UPDATE dbo.Authentication SET [LastLogin]=@lastLogin,[LastIPAddress]=@lastIPAddress,[LastDeviceId]=@lastDeviceId,[Token]=@token WHERE [Id]=@id;";

                response.LastDeviceId = request.DeviceId;
                response.LastIPAddress = request.IPAddress;
                response.LastLogin = DateTime.Now;
                response.Token = Guid.NewGuid().ToString();

                await writeConnection.ExecuteAsync(sql, new
                {
                    id = response.Id,
                    lastLogin = response.LastLogin,
                    lastIPAddress = response.LastIPAddress,
                    lastDeviceId = response.LastDeviceId,
                    token = response.Token
                }, uom.GetTransaction());
            }

            return response;
        }

        public async Task<LoginResponse> GetByIdAsync(int id)
        {
            var readConnection = await _session.GetReadOnlyConnectionAsync();
            string sql = @"SELECT TOP 1 [Id],[LoginName],[LastLogin],[LastIPAddress],[LastDeviceId],[Token] FROM dbo.Authentication WHERE [Id]=@id;";
            return await readConnection.QueryFirstOrDefaultAsync<LoginResponse>(sql, new
            {
                id
            });
        }
    }
}
