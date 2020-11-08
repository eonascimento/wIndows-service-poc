using System;
using Topshelf;

namespace FileConverterService
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(serviceConfig =>
            {
                serviceConfig.UseNLog();
                serviceConfig.Service<ConverterService>(serviceInstance =>
                {
                    serviceInstance.ConstructUsing(() => new ConverterService());
                    serviceInstance.WhenStarted(execute => execute.Start());
                    serviceInstance.WhenStopped(execute => execute.Stop());
                    serviceInstance.WhenPaused(execute => execute.Pause());
                    serviceInstance.WhenContinued(execute => execute.Continue());
                    serviceInstance.WhenCustomCommandReceived((execute,hostControl, commandNumber) =>
                        execute.CustomCommand(commandNumber));

                });
                serviceConfig.EnableServiceRecovery(recoveryOption =>
                {
                    recoveryOption.RestartService(1);
                    recoveryOption.RestartComputer(60, "Demonstração Serviço Windows");
                    recoveryOption.RunProgram(5, "c:\\algumprograma.exe");
                });
                serviceConfig.EnablePauseAndContinue();
                serviceConfig.SetServiceName("ServicoWindowsConversorArquivo");
                serviceConfig.SetDisplayName("Demonstração Serviço Windows");
                serviceConfig.SetDescription("Serviço do windows para converter arquivo");

                serviceConfig.StartAutomatically();
            });
        }
    }
}
