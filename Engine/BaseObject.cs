using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public abstract class BaseObject
    {
        #region Public methods

        public abstract void ClockTick();
        public virtual void Initialize()
        {

        }
        public void Destroy()
        {
            if (!mIsDestroyed)
            {
                mIsDestroyed = true;
                DestroyActions();
            }
        }
        protected virtual void DestroyActions()
        {
            RaiseDestroyedEvent();
            RaiseRoomActionEvent(ERoomAction.RemoveObject, this);
        }

        #endregion
        #region Protected methods

        protected void RaiseRoomActionEvent(ERoomAction aAction, object arg)
        {
            if (RoomActionEvent != null)
            {
                RoomActionEvent(aAction, arg);
            }
        }
        protected void RaiseDestroyedEvent()
        {
            if (DestroyedEvent != null)
            {
                DestroyedEvent(this);
            }
        }

        #endregion
        #region Properties

        public bool IsDestroyed
        {
            get
            {
                return mIsDestroyed;
            }
        }
        public IGameRoom GameRoom
        {
            get;
            set;
        }

        #endregion
        #region Events
        
        public event RoomActionRequest RoomActionEvent;
        public event Destruction DestroyedEvent;

        #endregion
        #region Delegates

        public delegate void RoomActionRequest(ERoomAction aAction, object arg);
        public delegate void ActionWithObject<T>(T o) where T : BaseObject;
        public delegate void Destruction(BaseObject aSender);

        #endregion
        #region Members

        protected Random mRandom = new Random();
        private bool mIsDestroyed = false;

        #endregion
    }
}
