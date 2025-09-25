using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using ContaBancariaMVC.Models;

namespace ContaBancariaMVC.Controllers
{
    public class ContaController : Controller
    {
        // Simulação de banco de dados em memória
        private static List<ContaBancaria> contas = new List<ContaBancaria>
        {
            new ContaBancaria { Id = 1, NumeroConta = "12345-6", Titular = "João Silva", Saldo = 1000.00m, DataCriacao = DateTime.Now },
            new ContaBancaria { Id = 2, NumeroConta = "23456-7", Titular = "Maria Santos", Saldo = 2500.50m, DataCriacao = DateTime.Now }
        };

        private static List<Operacao> operacoes = new List<Operacao>();
        private static int nextId = 3;

        // GET: Conta
        public IActionResult Index()
        {
            return View(contas);
        }

        // GET: Conta/Details/5
        public IActionResult Details(int id)
        {
            var conta = contas.FirstOrDefault(c => c.Id == id);
            if (conta == null)
            {
                return NotFound();
            }

            // Carrega operações da conta
            var operacoesConta = operacoes.Where(o => o.ContaId == id).ToList();
            ViewBag.Operacoes = operacoesConta;

            return View(conta);
        }

        // GET: Conta/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Conta/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ContaBancaria conta)
        {
            if (ModelState.IsValid)
            {
                conta.Id = nextId++;
                conta.DataCriacao = DateTime.Now;
                contas.Add(conta);

                TempData["MensagemSucesso"] = "Conta criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            return View(conta);
        }

        // GET: Conta/Deposito/5
        public IActionResult Deposito(int id)
        {
            var conta = contas.FirstOrDefault(c => c.Id == id);
            if (conta == null)
            {
                return NotFound();
            }

            ViewBag.Conta = conta;
            return View();
        }

        // POST: Conta/Deposito/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deposito(int id, decimal valor)
        {
            var conta = contas.FirstOrDefault(c => c.Id == id);
            if (conta == null)
            {
                return NotFound();
            }

            if (valor <= 0)
            {
                ModelState.AddModelError("valor", "O valor do depósito deve ser maior que zero");
            }

            if (ModelState.IsValid)
            {
                conta.Saldo += valor;

                // Registra a operação
                var operacao = new Operacao
                {
                    Id = operacoes.Count + 1,
                    ContaId = id,
                    Tipo = TipoOperacao.Deposito,
                    Valor = valor,
                    DataOperacao = DateTime.Now,
                    Descricao = $"Depósito realizado"
                };
                operacoes.Add(operacao);

                TempData["MensagemSucesso"] = $"Depósito de R$ {valor:F2} realizado com sucesso!";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            ViewBag.Conta = conta;
            return View();
        }

        // GET: Conta/Saque/5
        public IActionResult Saque(int id)
        {
            var conta = contas.FirstOrDefault(c => c.Id == id);
            if (conta == null)
            {
                return NotFound();
            }

            ViewBag.Conta = conta;
            return View();
        }

        // POST: Conta/Saque/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Saque(int id, decimal valor)
        {
            var conta = contas.FirstOrDefault(c => c.Id == id);
            if (conta == null)
            {
                return NotFound();
            }

            if (valor <= 0)
            {
                ModelState.AddModelError("valor", "O valor do saque deve ser maior que zero");
            }
            else if (valor > conta.Saldo)
            {
                ModelState.AddModelError("valor", "Saldo insuficiente para realizar o saque");
            }

            if (ModelState.IsValid)
            {
                conta.Saldo -= valor;

                // Registra a operação
                var operacao = new Operacao
                {
                    Id = operacoes.Count + 1,
                    ContaId = id,
                    Tipo = TipoOperacao.Saque,
                    Valor = valor,
                    DataOperacao = DateTime.Now,
                    Descricao = $"Saque realizado"
                };
                operacoes.Add(operacao);

                TempData["MensagemSucesso"] = $"Saque de R$ {valor:F2} realizado com sucesso!";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            ViewBag.Conta = conta;
            return View();
        }
    }
}
