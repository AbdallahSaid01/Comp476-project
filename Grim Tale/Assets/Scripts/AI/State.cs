using System.Linq;
using UnityEngine;

namespace AI
{
    public class State
    {
        public StateName stateName;

        protected StateEvent stage;
        protected State nextState;
        protected Enemy enemy;

        public State(Enemy enemy)
        {
            stage = StateEvent.Enter;
            this.enemy = enemy;
        }

        public virtual void Enter() { stage = StateEvent.Update; }
        public virtual void Update() { stage = StateEvent.Update; }
        public virtual void Exit() { stage = StateEvent.Exit; }

        // Processes the next state and stage
        public State Process()
        {
            if (stage == StateEvent.Enter) Enter();
            if (stage == StateEvent.Update) Update();
            if(stage == StateEvent.Exit)
            {
                Exit();
                return nextState;
            }

            return this;
        }
    }

    public enum StateName { Idle, Chase, Formation }
    public enum StateEvent { Enter, Update, Exit }
}