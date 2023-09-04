using System;
using System.Collections.Generic;

public class Data
{
    public int Dia { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }

    public Data(int dia, int mes, int ano)
    {
        Dia = dia;
        Mes = mes;
        Ano = ano;
    }

    public static bool TryParse(string dataStr, out Data data)
    {
        data = null;

        string[] partes = dataStr.Split('/');
        if (partes.Length == 3 && int.TryParse(partes[0], out int dia) &&
            int.TryParse(partes[1], out int mes) && int.TryParse(partes[2], out int ano))
        {
            data = new Data(dia, mes, ano);
            return true;
        }

        return false;
    }

    public override string ToString()
    {
        return $"{Dia:D2}/{Mes:D2}/{Ano:D4}";
    }
}




public class Telefone
{
    public string Tipo { get; set; }
    public string Numero { get; set; }
    public bool Principal { get; set; }
}

public class Contato
{
    public string Email { get; set; }
    public string Nome { get; set; }
    public Data DtNasc { get; set; }
    public List<Telefone> Telefones { get; } = new List<Telefone>();

    public int GetIdade()
    {
        DateTime hoje = DateTime.Now;
        int idade = hoje.Year - DtNasc.Ano;
        if (hoje.Month < DtNasc.Mes || (hoje.Month == DtNasc.Mes && hoje.Day < DtNasc.Dia))
        {
            idade--;
        }
        return idade;
    }

    public void AdicionarTelefone(Telefone telefone)
    {
        Telefones.Add(telefone);
    }

    public string GetTelefonePrincipal()
    {
        foreach (var telefone in Telefones)
        {
            if (telefone.Principal)
            {
                return telefone.Numero;
            }
        }
        return "Nenhum telefone principal encontrado";
    }

    public override string ToString()
    {
        string telefonePrincipal = GetTelefonePrincipal();
        return $"Nome: {Nome}\nEmail: {Email}\nData de Nascimento: {DtNasc}\nTelefone Principal: {telefonePrincipal}";
    }

    public override bool Equals(object obj)
    {
        if (obj is Contato contato)
        {
            return Email.Equals(contato.Email, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }
}

public class Contatos
{
    private readonly List<Contato> agenda = new List<Contato>();

    public bool Adicionar(Contato contato)
    {
        if (!agenda.Contains(contato))
        {
            agenda.Add(contato);
            return true;
        }
        return false;
    }

    public Contato Pesquisar(Contato contato)
    {
        foreach (var c in agenda)
        {
            if (c.Equals(contato))
            {
                return c;
            }
        }
        return null;
    }

    public bool Alterar(Contato contato)
    {
        int index = agenda.IndexOf(contato);
        if (index != -1)
        {
            agenda[index] = contato;
            return true;
        }
        return false;
    }

    public bool Remover(Contato contato)
    {
        return agenda.Remove(contato);
    }

    public void ListarContatos()
    {
        foreach (var contato in agenda)
        {
            Console.WriteLine(contato.ToString());
            Console.WriteLine("------------------------");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Contatos contatos = new Contatos();

        while (true)
        {
            Console.WriteLine("Escolha uma opção:");
            Console.WriteLine("0. Sair");
            Console.WriteLine("1. Adicionar contato");
            Console.WriteLine("2. Adicionar telefone no contato");
            Console.WriteLine("3. Pesquisar contato");
            Console.WriteLine("4. Alterar contato");
            Console.WriteLine("5. Remover contato");
            Console.WriteLine("6. Listar contatos");

            int opcao;
            if (int.TryParse(Console.ReadLine(), out opcao))
            {
                switch (opcao)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        AdicionarContato(contatos);
                        break;
                    case 2:
                        AdicionarTelefone(contatos);
                        break;
                    case 3:
                        PesquisarContato(contatos);
                        break;
                    case 4:
                        AlterarContato(contatos);
                        break;
                    case 5:
                        RemoverContato(contatos);
                        break;
                    case 6:
                        ListarContatos(contatos);
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Opção inválida. Tente novamente.");
            }
        }
    }

    static void AdicionarContato(Contatos contatos)
    {
        Console.Write("Digite o nome do contato: ");
        string nome = Console.ReadLine();
        Console.Write("Digite o email do contato: ");
        string email = Console.ReadLine();
        Console.Write("Digite a data de nascimento (dd/mm/aaaa): ");
        string dataNascimentoStr = Console.ReadLine();
        if (Data.TryParse(dataNascimentoStr, out Data dataNascimento))
        {
            Contato novoContato = new Contato
            {
                Nome = nome,
                Email = email,
                DtNasc = dataNascimento
            };
            contatos.Adicionar(novoContato);
            Console.WriteLine("Contato adicionado com sucesso!");
        }
        else
        {
            Console.WriteLine("Formato de data inválido.");
        }
    }

    static void AdicionarTelefone(Contatos contatos)
    {
        Console.Write("Digite o email do contato: ");
        string email = Console.ReadLine();
        Contato contato = new Contato { Email = email };
        Contato encontrado = contatos.Pesquisar(contato);

        if (encontrado != null)
        {
            Console.Write("Digite o tipo do telefone: ");
            string tipo = Console.ReadLine();
            Console.Write("Digite o número do telefone: ");
            string numero = Console.ReadLine();
            Console.Write("É o telefone principal? (S/N): ");
            string principalStr = Console.ReadLine();
            bool principal = principalStr.Equals("S", StringComparison.OrdinalIgnoreCase);

            Telefone telefone = new Telefone
            {
                Tipo = tipo,
                Numero = numero,
                Principal = principal
            };
            encontrado.AdicionarTelefone(telefone);
            Console.WriteLine("Telefone adicionado com sucesso!");
        }
        else
        {
            Console.WriteLine("Contato não encontrado.");
        }
    }

    static void PesquisarContato(Contatos contatos)
    {
        Console.Write("Digite o email do contato: ");
        string email = Console.ReadLine();
        Contato contato = new Contato { Email = email };
        Contato encontrado = contatos.Pesquisar(contato);

        if (encontrado != null)
        {
            Console.WriteLine(encontrado.ToString());
        }
        else
        {
            Console.WriteLine("Contato não encontrado.");
        }
    }

    static void AlterarContato(Contatos contatos)
    {
        Console.Write("Digite o email do contato que deseja alterar: ");
        string email = Console.ReadLine();
        Contato contato = new Contato { Email = email };
        Contato encontrado = contatos.Pesquisar(contato);

        if (encontrado != null)
        {
            Console.Write("Digite o novo nome do contato: ");
            string nome = Console.ReadLine();
            Console.Write("Digite o novo email do contato: ");
            string novoEmail = Console.ReadLine();
            Console.Write("Digite a nova data de nascimento (dd/mm/aaaa): ");
            string dataNascimentoStr = Console.ReadLine();
            if (Data.TryParse(dataNascimentoStr, out Data dataNascimento))
            {
                encontrado.Nome = nome;
                encontrado.Email = novoEmail;
                encontrado.DtNasc = dataNascimento;
                Console.WriteLine("Contato alterado com sucesso!");
            }
            else
            {
                Console.WriteLine("Formato de data inválido.");
            }
        }
        else
        {
            Console.WriteLine("Contato não encontrado.");
        }
    }

    static void RemoverContato(Contatos contatos)
    {
        Console.Write("Digite o email do contato que deseja remover: ");
        string email = Console.ReadLine();
        Contato contato = new Contato { Email = email };
        bool removido = contatos.Remover(contato);

        if (removido)
        {
            Console.WriteLine("Contato removido com sucesso!");
        }
        else
        {
            Console.WriteLine("Contato não encontrado.");
        }
    }

    static void ListarContatos(Contatos contatos)
    {
        Console.WriteLine("Lista de Contatos:");
        contatos.ListarContatos();
    }
}