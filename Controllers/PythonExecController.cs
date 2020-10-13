using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IronPythonAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PythonExecController : ControllerBase
    {
        private readonly ILogger<PythonExecController> _logger;

        public PythonExecController(ILogger<PythonExecController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<String> Get()
        {
            return PythonExec.GetInstance().GetAllProgramsNames();
        }

        [HttpGet("{programName}")]
        public String Get(String programName)
        {
            String saida;
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();

            PythonExec.PythonProgram program = PythonExec.GetInstance().GetProgram(programName);

            program.Execute();
            saida = (String) program.GetVariable("saida");

            watch.Stop();

            Console.WriteLine("Tempo de execução do script " + programName + " " + watch.ElapsedMilliseconds + "ms");

            return saida;
        }

        [HttpPost("{programName}")]
        public String Post(String programName, [FromBody] Object entradaJson)
        {
            String saida;
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();
            PythonExec.PythonProgram program = PythonExec.GetInstance().GetProgram(programName);

            program.SetVariable("entradaJson",entradaJson.ToString());
            program.Execute();
            saida = (String) program.GetVariable("saida");

            watch.Stop();

            Console.WriteLine("Tempo de execução do script " + programName + " " + watch.ElapsedMilliseconds + "ms");

            return saida;
        }
    }
}
