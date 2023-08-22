// Copyright (c) 2015 - 2023 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

//.........................
//.....Generated Class.....
//.........................
//.......Do not edit.......
//.........................

using UnityEngine;
// ReSharper disable All

namespace Doozy.Runtime.Signals
{
    public partial class Signal
    {
        public static bool Send(StreamId.Global id, string message = "") => SignalsService.SendSignal(nameof(StreamId.Global), id.ToString(), message);
        public static bool Send(StreamId.Global id, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.Global), id.ToString(), signalSource, message);
        public static bool Send(StreamId.Global id, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.Global), id.ToString(), signalProvider, message);
        public static bool Send(StreamId.Global id, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.Global), id.ToString(), signalSender, message);
        public static bool Send<T>(StreamId.Global id, T signalValue, string message = "") => SignalsService.SendSignal(nameof(StreamId.Global), id.ToString(), signalValue, message);
        public static bool Send<T>(StreamId.Global id, T signalValue, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.Global), id.ToString(), signalValue, signalSource, message);
        public static bool Send<T>(StreamId.Global id, T signalValue, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.Global), id.ToString(), signalValue, signalProvider, message);
        public static bool Send<T>(StreamId.Global id, T signalValue, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.Global), id.ToString(), signalValue, signalSender, message);

        public static bool Send(StreamId.Level id, string message = "") => SignalsService.SendSignal(nameof(StreamId.Level), id.ToString(), message);
        public static bool Send(StreamId.Level id, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.Level), id.ToString(), signalSource, message);
        public static bool Send(StreamId.Level id, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.Level), id.ToString(), signalProvider, message);
        public static bool Send(StreamId.Level id, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.Level), id.ToString(), signalSender, message);
        public static bool Send<T>(StreamId.Level id, T signalValue, string message = "") => SignalsService.SendSignal(nameof(StreamId.Level), id.ToString(), signalValue, message);
        public static bool Send<T>(StreamId.Level id, T signalValue, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.Level), id.ToString(), signalValue, signalSource, message);
        public static bool Send<T>(StreamId.Level id, T signalValue, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.Level), id.ToString(), signalValue, signalProvider, message);
        public static bool Send<T>(StreamId.Level id, T signalValue, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.Level), id.ToString(), signalValue, signalSender, message);
   
    }

    public partial class StreamId
    {
        public enum Global
        {
            HapticSelection
        }

        public enum Level
        {
            LevelComplete,
            LevelFail,
            LevelStart
        }         
    }
}
