// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace QuikSharp
{
    internal static class ServiceManager
    {
        private static QuikService quikService;

        public static void StartServices()
        {
            quikService = QuikService.Create(Quik.DefaultPort, Quik.DefaultHost);
        }

        public static void StopServices()
        {
        }

        public static void RestartServices()
        {
            StopServices();
            StartServices();
        }
    }
}