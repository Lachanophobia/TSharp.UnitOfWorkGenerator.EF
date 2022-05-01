﻿using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TSharp.UnitOfWorkGenerator.Samples.Entities;
using TSharp.UnitOfWorkGenerator.Samples.Repositories.IRepository;

namespace TSharp.UnitOfWorkGenerator.Samples.Repositories.Repository
{
    public class SP_Call : ISP_Call
    {

        private readonly TSharpContext _db;
        private static string connectionString = "";

        public SP_Call(TSharpContext db)
        {
            _db = db;
            connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        #region asynchronous methods

        /// <inheritdoc/>
        public async Task ExecuteAsync(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null)
        {
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            await dbConnection.ExecuteAsync(procedureName, param, transaction: transaction, commandTimeout: commandTimeout, commandType: CommandType.StoredProcedure);

            if (connection == null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> ListAsync<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null)
        {
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            var result = await dbConnection.QueryAsync<T>(procedureName, param, commandType: CommandType.StoredProcedure, transaction: transaction, commandTimeout: commandTimeout);

            if (connection == null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> ListAsync<T1, T2>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null)
        {
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            var result = await connection.QueryMultipleAsync(procedureName, commandType: CommandType.StoredProcedure, transaction: transaction, commandTimeout: commandTimeout);
            var item1 = (await result.ReadAsync<T1>()).ToList();
            var item2 = (await result.ReadAsync<T2>()).ToList();

            if (item1 != null && item2 != null)
            {
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
            }

            if (connection == null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }

            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
        }

        /// <inheritdoc/>
        public async Task<T> OneRecordAsync<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null)
        {
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            var value = dbConnection.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure, transaction: transaction, commandTimeout: commandTimeout);
            var result = (T)Convert.ChangeType(value.FirstOrDefault(), typeof(T));

            if (connection == null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<T> SingleAsync<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null)
        {
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            var result = (T)Convert.ChangeType(await connection.ExecuteScalarAsync<T>(procedureName, param, commandType: CommandType.StoredProcedure, transaction: transaction, commandTimeout: commandTimeout), typeof(T));

            if (connection == null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }

            return result;
        }
        #endregion

        #region synchronous methods
        /// <inheritdoc/>
        public void Execute(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null)
        {
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            dbConnection.Execute(procedureName, param, transaction: transaction, commandTimeout: commandTimeout, commandType: CommandType.StoredProcedure);

            if (connection == null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }
        }

        /// <inheritdoc/>
        public IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null)
        {
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            var result = dbConnection.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure, transaction: transaction, commandTimeout: commandTimeout);

            if (connection == null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }

            return result;
        }

        /// <inheritdoc/>
        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null)
        {
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            var result = connection.QueryMultiple(procedureName, commandType: CommandType.StoredProcedure, transaction: transaction, commandTimeout: commandTimeout);
            var item1 = result.Read<T1>().ToList();
            var item2 = result.Read<T2>().ToList();

            if (item1 != null && item2 != null)
            {
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
            }

            if (connection == null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }

            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
        }

        /// <inheritdoc/>
        public T OneRecord<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null)
        {
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            var value = dbConnection.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure, transaction: transaction, commandTimeout: commandTimeout);
            var result = (T)Convert.ChangeType(value.FirstOrDefault(), typeof(T));

            if (connection == null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }

            return result;
        }

        /// <inheritdoc/>
        public T Single<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null)
        {
            IDbConnection dbConnection = connection ?? new SqlConnection(connectionString);
            dbConnection.Open();

            var result = (T)Convert.ChangeType(connection.ExecuteScalar<T>(procedureName, param, commandType: CommandType.StoredProcedure, transaction: transaction, commandTimeout: commandTimeout), typeof(T));

            if (connection == null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }

            return result;
        }
        #endregion


    }
}
