using System;
using System.ComponentModel.DataAnnotations;

namespace ContaBancariaMVC.Models
{
    public class ContaBancaria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O número da conta é obrigatório")]
        [Display(Name = "Número da Conta")]
        public string NumeroConta { get; set; }

        [Required(ErrorMessage = "O nome do titular é obrigatório")]
        [Display(Name = "Titular")]
        public string Titular { get; set; }

        [Display(Name = "Saldo")]
        [DataType(DataType.Currency)]
        public decimal Saldo { get; set; }

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; }
    }

    public class Operacao
    {
        public int Id { get; set; }

        [Required]
        public int ContaId { get; set; }

        [Required(ErrorMessage = "O tipo de operação é obrigatório")]
        [Display(Name = "Tipo de Operação")]
        public TipoOperacao Tipo { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
        [Display(Name = "Valor")]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        [Display(Name = "Data da Operação")]
        public DateTime DataOperacao { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        // Navigation property
        public virtual ContaBancaria Conta { get; set; }
    }

    public enum TipoOperacao
    {
        Deposito,
        Saque
    }
}
