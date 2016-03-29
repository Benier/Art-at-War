/*using UnityEngine;
using System.Collections;

public class QLearner
{
    int stride;
    LearningProblem problem;
    public QLearner(LearningProblem* problem,
                       float alpha, float gamma, float rho, float nu)
            :
            problem(problem), alpha(alpha), gamma(gamma), rho(rho), nu(nu)
    {
        stride = problem->getActionCount();
        unsigned size = problem->getStateCount() * stride;
        qvalues = new real[size];
        memset(qvalues, 0, sizeof(real) * size);
    }

    QLearner::~QLearner()
    {
        delete[] qvalues;
    }

    real QLearner::getQValue(LearningProblemState* state,
                             LearningProblemAction* action)
    {
        return qvalues[state->index * stride + action->index];
    }

    void QLearner::storeQValue(LearningProblemState* state,
                               LearningProblemAction* action,
                               real qvalue)
    {
        qvalues[state->index * stride + action->index] = qvalue;
    }

    real QLearner::getBestQValue(LearningProblemState* state)
    {
        real best = (real)0;
        for (unsigned i = 0; i < stride; i++)
        {
            real q = qvalues[state->index * stride + i];
            if (q > best) best = q;
        }
        return best;
    }

    LearningProblemAction*
    QLearner::getBestAction(LearningProblemState* state)
    {
        // Get the actions
        LearningProblemAction* action = problem->getActions(state);

        // Check them in turn
        real best = (real)0;
        LearningProblemAction* bestAction = action;
        while (action != NULL)
        {
            real q = getQValue(state, action);
            if (q > best)
            {
                best = q;
                bestAction = action;
            }

            action = action->next;
        }

        return bestAction;
    }

    LearningProblemState*
    QLearner::doLearningIteration(LearningProblemState* state)
    {
        // Pick a new state once in a while
        if (randomReal() < nu)
        {
            state = problem->getRandomState();
        }

        // Get the list of actions
        LearningProblemAction* actions = problem->getActions(state);
        LearningProblemAction* action = NULL;

        // Check if we should use a random action, or the best one
        if (randomReal() < rho)
        {
            unsigned randPos = randomInt(actions->getCount());
            action = actions->getAtPositionInList(randPos);
        }
        else {
            action = getBestAction(state);
        }

        // Make sure we've got something to do
        if (action != NULL)
        {
            // Carry out the action
            LearningProblemActionResult result =
                problem->getResult(state, action);

            // Get the current q value
            real q = getQValue(state, action);

            // Get the q of the best action from the new state
            real maxQ = getBestQValue(result.state);

            // recalculate the q
            q = ((real)1.0 - alpha) * q + alpha * (result.reward + gamma * maxQ);

            // Store the new Q value
            storeQValue(state, action, q);

            return result.state;
        }
        // Otherwise we need to get a new state - we've reached the
        // end of the road.
        else
        {
            return problem->getRandomState();
        }
    }

    void QLearner::learn(unsigned iterations)
    {
        // We choose a random place to start
        LearningProblemState* state = problem->getInitialState();

        // Then walk a little
        for (unsigned i = 0; i < iterations; i++)
        {
            state = doLearningIteration(state);
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
*/