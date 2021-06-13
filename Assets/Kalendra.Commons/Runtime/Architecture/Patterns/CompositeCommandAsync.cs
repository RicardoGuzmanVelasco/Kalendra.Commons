using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kalendra.Commons.Runtime.Architecture.Patterns
{
    public abstract class CompositeCommandAsync : ICommandAsync
    {
        readonly IEnumerable<ICommandAsync> commands;

        protected CompositeCommandAsync(IEnumerable<ICommandAsync> commands)
        {
            this.commands = commands;
        }

        public async Task Execute()
        {
            foreach(var commandAsync in commands)
                await commandAsync.Execute();
        }

        public async Task Undo()
        {
            foreach(var commandAsync in commands)
                await commandAsync.Undo();
        }
    }

    public class MacroCompositeCommandAsync : CompositeCommandAsync
    {
        public MacroCompositeCommandAsync(IEnumerable<ICommandAsync> commands) : base(commands) { }
    }
}