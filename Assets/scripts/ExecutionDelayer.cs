using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionDelayer : GenericSingletonBehaviour<ExecutionDelayer>
{
    public override string GetName()
    {
        return "ExecutionDelayer";
    }

    public delegate void ExecutionDelegate();

    private struct ExecutionData
    {
        public ExecutionDelegate ExecuteFunction;
        public float ExecuteAfterGameTime;
    }

    private List<ExecutionData> ExecutionList = new List<ExecutionData>();
    private List<ExecutionData> ExecutionListBuffer = new List<ExecutionData>();
    private float lastFrameTime = 0;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        lastFrameTime = Time.unscaledTime;
    }

    public void ExecuteNextFrame(ExecutionDelegate func)
    {
        lock (this)
        {
            ExecutionListBuffer.Add(new ExecutionData()
            {
                ExecuteFunction = func,
                ExecuteAfterGameTime = -1
            });
        }
    }

    public void ExecuteInSeconds(float time, ExecutionDelegate func)
    {
        lock (this)
        {
            ExecutionListBuffer.Add(new ExecutionData()
            {
                ExecuteFunction = func,
                ExecuteAfterGameTime = lastFrameTime + time
            });
        }
    }

    // Update is called once per frame
    private void Update()
    {
        for (int execIndex = ExecutionList.Count - 1; execIndex >= 0; --execIndex)
        {
            if (ExecutionList[execIndex].ExecuteAfterGameTime < Time.unscaledTime)
            {
                try
                {
                    ExecutionList[execIndex].ExecuteFunction();
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("executiondelay had an error in one of its functions: " + ex.ToString());
                }
                ExecutionList.RemoveAt(execIndex);
            }
        }
        lastFrameTime = Time.unscaledTime;
    }

    private void LateUpdate()
    {
        lock (this)
        {
            ExecutionList.AddRange(ExecutionListBuffer);
            ExecutionListBuffer.Clear();
        }
    }
}