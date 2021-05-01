using System.Collections.Generic;

namespace Kalendra.Commons.Runtime.Architecture.Patterns
{
    public abstract class CompositeCommand : ICommand
    {
        readonly IEnumerable<ICommand> commands;

        protected CompositeCommand(IEnumerable<ICommand> commands)
        {
            this.commands = commands;
        }

        public void Execute()
        {
            foreach(var command in commands)
                command.Execute();
        }

        public void Undo()
        {
            foreach(var command in commands)
                command.Undo();
        }
    }

    class MacroCompositeCommand : CompositeCommand
    {
        public MacroCompositeCommand(IEnumerable<ICommand> commands) : base(commands) { }
    }
}