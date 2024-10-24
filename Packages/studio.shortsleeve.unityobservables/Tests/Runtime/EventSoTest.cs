using System;
using NUnit.Framework;
using UnityEngine;

namespace Studio.ShortSleeve.UnityObservables
{
    public class EventSoTest
    {
        private EventAssetInt _eventAssetInt;
        private EventAssetVoid _eventAssetVoid;
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
            _eventAssetInt = ScriptableObject.CreateInstance<EventAssetInt>();
            _eventAssetVoid = ScriptableObject.CreateInstance<EventAssetVoid>();
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
            _eventAssetVoid.Subscribe(_eventHandlerVoid);
            _eventAssetVoid.Subscribe(_eventHandlerInterfaceVoid);
            _eventAssetVoid.Raise();

            // Verify
            Assert.AreEqual(true, _eventHandlerVoidCalled);
            Assert.AreEqual(true, _eventHandlerInterfaceVoid.Invoked);

            // Reset
            ResetEventHandlerVoid();
            _eventHandlerInterfaceVoid.Reset();

            // Invoke
            _eventAssetVoid.Unsubscribe(_eventHandlerVoid);
            _eventAssetVoid.Unsubscribe(_eventHandlerInterfaceVoid);
            _eventAssetVoid.Raise();

            // Verify
            Assert.AreEqual(false, _eventHandlerVoidCalled);
            Assert.AreEqual(false, _eventHandlerInterfaceVoid.Invoked);
        }

        [Test]
        public void TestSubscribeRaiseUnsubscribeRaiseEventGeneric()
        {
            // Invoke
            int setValue = 10;
            _eventAssetInt.Subscribe(_eventHandlerInt);
            _eventAssetInt.Subscribe(_eventHandlerInterfaceInt);
            _eventAssetInt.Raise(setValue);

            // Verify 
            Assert.AreEqual(true, _eventHandlerIntCalled);
            Assert.AreEqual(true, _eventHandlerInterfaceInt.Invoked);
            Assert.AreEqual(setValue, EventHandlerIntValue);
            Assert.AreEqual(setValue, _eventHandlerInterfaceInt.Value);

            // Reset
            ResetEventHandlerInt();
            _eventHandlerInterfaceInt.Reset();

            // Invoke
            _eventAssetInt.Unsubscribe(_eventHandlerInt);
            _eventAssetInt.Unsubscribe(_eventHandlerInterfaceInt);
            _eventAssetInt.Raise(20);

            // Verify
            Assert.AreEqual(false, _eventHandlerIntCalled);
            Assert.AreEqual(false, _eventHandlerInterfaceInt.Invoked);
            Assert.AreEqual(0, EventHandlerIntValue);
            Assert.AreEqual(0, _eventHandlerInterfaceInt.Value);
        }

        [Test]
        public void TestRaiseWithException()
        {
            _eventAssetVoid.Subscribe(() => throw new Exception());
            Assert.Throws<Exception>(() => _eventAssetVoid.Raise());
        }

        [Test]
        public void TestSubscribeRaiseRaise()
        {
            // Invoke
            _eventAssetVoid.Subscribe(_eventHandlerVoid);
            _eventAssetVoid.Subscribe(_eventHandlerInterfaceVoid);
            _eventAssetVoid.Raise();

            // Verify
            Assert.AreEqual(true, _eventHandlerVoidCalled);
            Assert.AreEqual(true, _eventHandlerInterfaceVoid.Invoked);

            // Reset
            ResetEventHandlerVoid();
            _eventHandlerInterfaceVoid.Reset();

            // Invoke
            _eventAssetVoid.Raise();

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