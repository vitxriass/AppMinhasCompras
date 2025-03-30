using MauiAppMinhasCompras.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        public Task<int> Insert(Produto p)
        {
            if (p.DataCadastro == default)
            {
                p.DataCadastro = DateTime.Now;
            }

            return _conn.InsertAsync(p);
        }

        public Task<int> Update(Produto p)
        {
            return _conn.UpdateAsync(p);
        }

        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * FROM Produto WHERE Descricao LIKE ?";
            return _conn.QueryAsync<Produto>(sql, "%" + q + "%");
        }

        public Task<List<Produto>> GetProdutosPorPeriodo(DateTime inicio, DateTime fim, string categoria = "")
        {
            inicio = inicio.Date;
            fim = fim.Date.AddDays(1).AddTicks(-1);

            string sql;

            if (string.IsNullOrEmpty(categoria))
            {
                sql = "SELECT * FROM Produto WHERE DataCadastro BETWEEN ? AND ?";
                return _conn.QueryAsync<Produto>(sql, inicio, fim);
            }
            else
            {
                sql = "SELECT * FROM Produto WHERE Categoria LIKE ? AND DataCadastro BETWEEN ? AND ?";
                return _conn.QueryAsync<Produto>(sql, "%" + categoria + "%", inicio, fim);
            }
        }
    }
}
