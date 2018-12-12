using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDictationInputReceiver
{
    void ReceiveDictationResult(string message);
    void ReceiveDictationHypothesis(string message);
}
