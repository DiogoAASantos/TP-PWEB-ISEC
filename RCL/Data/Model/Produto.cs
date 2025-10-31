using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyColl.RCL.Data.Model;

public enum EstadoProduto
{
    Listado,
    AVenda,
    Vendido,
    ParaAlugar
}

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal? Preco { get; set; }
    public EstadoProduto Estado { get; set; }
    public string Tipo { get; set; } = string.Empty; // "Colecionável" ou "Complemento"

    // Relações
    public ICollection<ProdutoCategoria> ProdutoCategorias { get; set; } = new List<ProdutoCategoria>();
}


