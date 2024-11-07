using AutoMapper;
using DVLD.Domain.Common;
using DVLD.Persistence.Handlers;
using Microsoft.Data.SqlClient;

namespace DVLD.Persistence.Repositories.Base
{
    public abstract class BaseRepository
    {
        protected readonly DataSendhandler _dataSendhandler;
        protected readonly IMapper _mapper;
        public BaseRepository(DataSendhandler dataSendhandler, IMapper mapper)
        {
            _dataSendhandler = dataSendhandler;
            _mapper = mapper;
        }


        protected async Task<TEntity?> GetEntityAsync<TEntity>(string storedProcedure, int id, string parameterName) where TEntity : BaseEntity?
        {
            TEntity? entity = null;

            await _dataSendhandler.Handle(storedProcedure, async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue($"@{parameterName}", id);
                Connection.Open();
                using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        entity = _mapper.Map<TEntity>(reader);
                        entity.Mode = Domain.Common.Enums.enMode.Update;
                    }
                }
            });

            return entity;
        }
        protected async Task<T?> GetAsync<T>(string storedProcedure, int id, string parameterName) where T : class
        {
            T? entity = null;

            await _dataSendhandler.Handle(storedProcedure, async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue($"@{parameterName}", id);
                Connection.Open();
                using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        entity = _mapper.Map<T>(reader);
                    }
                }
            });

            return entity;
        }
        protected async Task<IReadOnlyList<T>> ListAllAsync<T>(string storedProcedure) where T : class, new()
        {
            List<T> list = new();

            await _dataSendhandler.Handle(storedProcedure, async (Connection, Command) =>
            {
                Connection.Open();
                using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        list.Add(_mapper.Map<T>(reader));
                    }

                }
            });

            return list;
        }

        protected async Task<bool> DeleteAsync(string storedProcedure, int id, string parameterName = "Id")
        {
            int RowAffected = 0;
            await _dataSendhandler.Handle(storedProcedure, async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue($"@{parameterName}", id);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;

        }

        protected async Task<bool> IsUnique(string storedProcedure, string parameterValue, string parameterName, int? id)
        {
            bool IsUnique = false;
            await _dataSendhandler.Handle(storedProcedure, async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue($"@{parameterName}", parameterValue);
                if (id is not null)
                    Command.Parameters.AddWithValue("@Id", id);
                object? Result = await Command.ExecuteScalarAsync();

                IsUnique = Result is not null && (int)Result == 1;
            });

            return IsUnique;
        }

        protected async Task<bool> IsUnique(string storedProcedure, int parameterValue, string parameterName, int? id)
        {
            bool IsUnique = false;
            await _dataSendhandler.Handle(storedProcedure, async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue($"@{parameterName}", parameterValue);
                if (id is not null)
                    Command.Parameters.AddWithValue("@Id", id);
                object? Result = await Command.ExecuteScalarAsync();

                IsUnique = Result is not null && (int)Result == 1;
            });

            return IsUnique;
        }

        protected async Task<bool> CheckConditionAsync(string storedProcedure, int parameterValue, string parameterName = "Id")
        {
            bool IsFound = false;
            await _dataSendhandler.Handle(storedProcedure, async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue($"@{parameterName}", parameterValue);
                object? Result = await Command.ExecuteScalarAsync();

                IsFound = Result is not null && (int)Result == 1;
            });

            return IsFound;
        }

        protected async Task<bool> CheckConditionAsync(string storedProcedure, string parameterValue, string parameterName = "Id")
        {
            bool IsFound = false;
            await _dataSendhandler.Handle(storedProcedure, async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue($"@{parameterName}", parameterValue);
                object? Result = await Command.ExecuteScalarAsync();

                IsFound = Result is not null && (int)Result == 1;
            });

            return IsFound;
        }
    }
}
