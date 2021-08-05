using System;

namespace BasicSccExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Digite os changeSets que deseja aplicar no outros ambientes separando por vírgula e depois enter:");
            var changesSetsInput = Console.ReadLine();
            Console.WriteLine("Digite o tipo de Branch (0 - HML, 1 - PRD):");
            var tipoBranch = int.Parse(Console.ReadLine());
            Console.WriteLine("Digite o tipo de Branch (0 - Merge, 1 - RDM):");
            var tipoPlanilha = int.Parse(Console.ReadLine());

            var retorno = new TfsConnector.App.Service.TfsServiceConnection().Run(tipoPlanilha,tipoBranch,changesSetsInput);

            if (retorno.Retorno)
            {
                Console.ReadLine();
            }
        }
    }
}
