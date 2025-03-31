import numpy as np
import matplotlib.pyplot as plt

# Initialize lists to store results
episode_rewards = []
episode_lengths = []
success_rates = []

for episode in range(num_episodes):
    state = env.reset()
    done = False
    total_reward = 0
    steps = 0
    success = 0  # Track successful item collection

    while not done:
        action = agent.act(state)  # Get action from trained model
        next_state, reward, done, info = env.step(action)

        total_reward += reward
        steps += 1

        if info.get("success", False):  # Track success if applicable
            success = 1

    episode_rewards.append(total_reward)
    episode_lengths.append(steps)
    success_rates.append(success)

# Convert lists to NumPy arrays for easy analysis
episode_rewards = np.array(episode_rewards)
episode_lengths = np.array(episode_lengths)
success_rates = np.array(success_rates)
