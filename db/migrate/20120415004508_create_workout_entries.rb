class CreateWorkoutEntries < ActiveRecord::Migration
  def change
    create_table :workout_entries do |t|

      t.timestamps
    end
  end
end
