import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns
import numpy as np

# Function to visualize training results
def visualize_training_results(csv_path):
    # Read the CSV file
    df = pd.read_csv(csv_path)
    
    # Calculate success rate for each row
    df['SuccessRate'] = (df['Successes'] / df['Episode']) * 100
    
    # Group data by Episode milestone (100, 200, etc.)
    episode_milestones = df['Episode'].unique()
    
    # Create aggregated data by milestone
    agg_data = []
    for milestone in sorted(episode_milestones):
        milestone_df = df[df['Episode'] == milestone]
        
        agg_row = {
            'Episode': milestone,
            'AvgSuccesses': milestone_df['Successes'].mean(),
            'AvgCollisions': milestone_df['Collisions'].mean(),
            'AvgSteps': milestone_df['TotalSteps'].mean(),
            'AvgReward': milestone_df['AverageReward'].mean(),
            'SuccessRate': milestone_df['SuccessRate'].mean()
        }
        agg_data.append(agg_row)
    
    agg_df = pd.DataFrame(agg_data)
    
    # Set up the plots with improved style
    plt.style.use('ggplot')
    
    # Create a figure with 3 subplots
    fig = plt.figure(figsize=(18, 14))
    
    # 1. Success Rate plot
    ax1 = fig.add_subplot(3, 1, 1)
    ax1.plot(agg_df['Episode'], agg_df['SuccessRate'], 'o-', color='#4C72B0', linewidth=2, markersize=8)
    ax1.set_xlabel('Episodes', fontsize=12)
    ax1.set_ylabel('Success Rate (%)', fontsize=12)
    ax1.set_title('Success Rate Throughout Training', fontsize=16)
    ax1.grid(True, linestyle='--', alpha=0.7)
    
    # Add value labels to the points
    for i, rate in enumerate(agg_df['SuccessRate']):
        ax1.annotate(f"{rate:.1f}%", 
                    (agg_df['Episode'].iloc[i], rate),
                    textcoords="offset points", 
                    xytext=(0,10), 
                    ha='center',
                    fontsize=9)
    
    # 2. Success vs Collisions
    ax2 = fig.add_subplot(3, 1, 2)
    
    ax2.plot(agg_df['Episode'], agg_df['AvgSuccesses'], 'o-', label='Successes', color='#55A868', linewidth=2, markersize=8)
    ax2.plot(agg_df['Episode'], agg_df['AvgCollisions'], 'o-', label='Collisions', color='#C44E52', linewidth=2, markersize=8)
    
    ax2.set_xlabel('Episodes', fontsize=12)
    ax2.set_ylabel('Count', fontsize=12)
    ax2.set_title('Successes vs Collisions Throughout Training', fontsize=16)
    ax2.legend(fontsize=12)
    ax2.grid(True, linestyle='--', alpha=0.7)
    
    # 3. Average Reward
    ax3 = fig.add_subplot(3, 1, 3)
    ax3.plot(agg_df['Episode'], agg_df['AvgReward'], 'o-', color='#8172B3', linewidth=2, markersize=8)
    ax3.set_xlabel('Episodes', fontsize=12)
    ax3.set_ylabel('Average Reward', fontsize=12)
    ax3.set_title('Average Reward Throughout Training', fontsize=16)
    ax3.grid(True, linestyle='--', alpha=0.7)
    
    # Add value labels to the points
    for i, reward in enumerate(agg_df['AvgReward']):
        ax3.annotate(f"{reward:.1f}", 
                    (agg_df['Episode'].iloc[i], reward),
                    textcoords="offset points", 
                    xytext=(0,10), 
                    ha='center',
                    fontsize=9)
    
    plt.tight_layout()
    
    # Create a summary statistics table
    print("\n===== TRAINING SUMMARY =====")
    print(f"Total Episodes: {df['Episode'].iloc[-1]}")
    print(f"Final Success Rate: {agg_df['SuccessRate'].iloc[-1]:.2f}%")
    print(f"Final Average Reward: {agg_df['AvgReward'].iloc[-1]:.2f}")
    print(f"Average Steps per Episode: {(agg_df['AvgSteps'].iloc[-1] / agg_df['Episode'].iloc[-1]):.2f}")
    
    # Training trends
    success_trend = np.polyfit(range(len(agg_df)), agg_df['SuccessRate'], 1)[0]
    reward_trend = np.polyfit(range(len(agg_df)), agg_df['AvgReward'], 1)[0]
    
    trend_message = "IMPROVING" if success_trend > 0 else "DECLINING"
    print(f"Success Rate Trend: {trend_message} ({success_trend:.4f})")
    
    trend_message = "IMPROVING" if reward_trend > 0 else "DECLINING"
    print(f"Average Reward Trend: {trend_message} ({reward_trend:.4f})")
    
    plt.savefig('training_results_visualization.png', dpi=300, bbox_inches='tight')
    plt.show()
    
    return df, agg_df

# Example usage
if __name__ == "__main__":
    # Replace with your actual CSV file path
    csv_path = "C:/Users/user/Final Year Project/RL Project/Assets/Results/MLAgentResults/training_results_20250406_153505.csv"
    
    # Call the visualization function
    df, agg_df = visualize_training_results(csv_path)
    
    # Display the head of the raw data
    print("\nRaw Data Sample:")
    print(df.head())
    
    # Display the aggregated data
    print("\nAggregated Data by Episode Milestone:")
    print(agg_df)