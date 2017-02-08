using System;
using System.Collections.Generic;

namespace dpas.Net.Http.Mvc
{
    public interface IController
    {
        void Exec(IControllerContext context);
    }

    public partial class Controller : IController
    {
        public Controller()
        {
            InitCommandHandlers();
        }

        private Dictionary<string, Action<IControllerContext>> _CommandHandlers;

        protected virtual void Init(Dictionary<string, Action<IControllerContext>> commandHandlers)
        {
        }

        protected virtual void InitCommandHandlers()
        {
            if(_CommandHandlers == null)
            {
                _CommandHandlers = new Dictionary<string, Action<IControllerContext>>();
                Init(_CommandHandlers);
            }
        }

        public virtual void Exec(IControllerContext context)
        {
            string command = context.ControllerInfo.Action;
            try
            {
                Action<IControllerContext> action;
                if (_CommandHandlers.TryGetValue(context.ControllerInfo.Action, out action))
                    action(context);
                else
                    throw new Exception(Controller.Exception.IsNotSupported, command);
            }
            catch (Controller.Exception controllerException)
            {
                throw controllerException;
            }
            catch (System.Exception exception)
            {
                throw new Controller.Exception(Controller.Exception.CommandFailed, command, exception.ToString());
            }
        }

    }
}
