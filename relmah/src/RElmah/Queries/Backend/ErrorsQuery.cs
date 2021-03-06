﻿using System;
using System.Threading.Tasks;

namespace RElmah.Queries.Backend
{
    public class ErrorsQuery : IBackendQuery
    {
        public async Task<IDisposable> Run(RunTargets targets)
        {
            return targets.ErrorsInbox.GetErrorsStream().Subscribe(payload => 
                targets.BackendNotifier.Error(payload));
        }
    }
}
