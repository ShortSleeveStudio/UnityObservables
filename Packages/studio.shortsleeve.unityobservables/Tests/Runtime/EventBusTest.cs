using System;
using NUnit.Framework;
using UnityEngine;

namespace Studio.ShortSleeve.UnityObservables
{
    public class EventBusTest
    {
        private EventInt _eventInt;
        private EventVoid _eventVoid;
        private EventHandlerInterfaceInt _eventHandlerInterfaceInt;
        private EventHandlerInterfaceVoid _eventHandlerInterfaceVoid;
        private EventHandler _eventHandlerVoid;
        private EventHandler<int> _eventHandlerInt;
        private int _eventHandlerIntValue;
        private bool _eventHandlerIntCalled;
        private bool _eventHandlerVoidCalled;

        [SetUp]
        public void SetUp()
        {
            _eventHandlerVoidCalled = false;
            _eventInt = ScriptableObject.CreateInstance<EventInt>();
            _eventVoid = ScriptableObject.CreateInstance<EventVoid>();
            _eventHandlerInterfaceInt = new();
            _eventHandlerInterfaceVoid = new();
            _eventHandlerInt = payload =>
            {
                _eventHandlerIntValue = payload;
                _eventHandlerIntCalled = true;
            };
            _eventHandlerVoid = () => { _eventHandlerVoidCalled = true; };
        }

        [Test]
        public void TestSubscribeRaiseUnsubscribeRaiseEventVoid()
        {
            // Invoke
            _eventVoid.Subscribe(_eventHandlerVoid);
            _eventVoid.Subscribe(_eventHandlerInterfaceVoid);
            _eventVoid.Raise();

            // Verify
            Assert.AreEqual(true, _eventHandlerVoidCalled);
            Assert.AreEqual(true, _eventHandlerInterfaceVoid.Invoked);

            // Reset
            ResetEventHandlerVoid();
            _eventHandlerInterfaceVoid.Reset();

            // Invoke
            _eventVoid.Unsubscribe(_eventHandlerVoid);
            _eventVoid.Unsubscribe(_eventHandlerInterfaceVoid);
            _eventVoid.Raise();

            // Verify
            Assert.AreEqual(false, _eventHandlerVoidCalled);
            Assert.AreEqual(false, _eventHandlerInterfaceVoid.Invoked);
        }

        [Test]
        public void TestSubscribeRaiseUnsubscribeRaiseEventGeneric()
        {
            // Invoke
            int setValue = 10;
            _eventInt.Subscribe(_eventHandlerInt);
            _eventInt.Subscribe(_eventHandlerInterfaceInt);
            _eventInt.Raise(setValue);

            // Verify 
            Assert.AreEqual(true, _eventHandlerIntCalled);
            Assert.AreEqual(true, _eventHandlerInterfaceInt.Invoked);
            Assert.AreEqual(setValue, EventHandlerIntValue);
            Assert.AreEqual(setValue, _eventHandlerInterfaceInt.Value);

            // Reset
            ResetEventHandlerInt();
            _eventHandlerInterfaceInt.Reset();

            // Invoke
            _eventInt.Unsubscribe(_eventHandlerInt);
            _eventInt.Unsubscribe(_eventHandlerInterfaceInt);
            _eventInt.Raise(20);

            // Verify
            Assert.AreEqual(false, _eventHandlerIntCalled);
            Assert.AreEqual(false, _eventHandlerInterfaceInt.Invoked);
            Assert.AreEqual(0, EventHandlerIntValue);
            Assert.AreEqual(0, _eventHandlerInterfaceInt.Value);
        }

        [Test]
        public void TestRaiseWithException()
        {
            _eventVoid.Subscribe(() => throw new Exception());
            Assert.Throws<Exception>(() => _eventVoid.Raise());
        }
        
        [Test]
        public void TestSubscribeRaiseRaise()
        {
            // Invoke
            _eventVoid.Subscribe(_eventHandlerVoid);
            _eventVoid.Subscribe(_eventHandlerInterfaceVoid);
            _eventVoid.Raise();

            // Verify
            Assert.AreEqual(true, _eventHandlerVoidCalled);
            Assert.AreEqual(true, _eventHandlerInterfaceVoid.Invoked);

            // Reset
            ResetEventHandlerVoid();
            _eventHandlerInterfaceVoid.Reset();

            // Invoke
            _eventVoid.Raise();

            // Verify
            Assert.AreEqual(true, _eventHandlerVoidCalled);
            Assert.AreEqual(true, _eventHandlerInterfaceVoid.Invoked);
        }

        #region Helpers

        private int EventHandlerIntValue => _eventHandlerIntValue;

        private void ResetEventHandlerInt()
        {
            _eventHandlerIntValue = 0;
            _eventHandlerIntCalled = false;
        }

        private void ResetEventHandlerVoid() => _eventHandlerVoidCalled = false;

        class EventHandlerInterfaceVoid : IEventHandler
        {
            private bool _invoked;
            public bool Invoked => _invoked;
            public void Reset() => _invoked = false;
            public void HandleEvent() => _invoked = true;
        }

        class EventHandlerInterfaceInt : IEventHandler<int>
        {
            private int _value;
            private bool _invoked;

            public bool Invoked => _invoked;
            public int Value => _value;

            public void Reset()
            {
                _value = 0;
                _invoked = false;
            }

            public void HandleEvent(int payload)
            {
                _invoked = true;
                _value = payload;
            }
        }

        #endregion
    }
}