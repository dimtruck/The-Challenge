require 'spec_helper'

describe "workout_entries/show" do
  before(:each) do
    @workout_entry = assign(:workout_entry, stub_model(WorkoutEntry))
  end

  it "renders attributes in <p>" do
    render
    # Run the generator again with the --webrat flag if you want to use webrat matchers
  end
end
