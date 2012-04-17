require 'spec_helper'

describe "workout_entries/index" do
  before(:each) do
    assign(:workout_entries, [
      stub_model(WorkoutEntry),
      stub_model(WorkoutEntry)
    ])
  end

  it "renders a list of workout_entries" do
    render
    # Run the generator again with the --webrat flag if you want to use webrat matchers
  end
end
