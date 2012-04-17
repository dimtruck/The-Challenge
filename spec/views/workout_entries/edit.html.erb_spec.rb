require 'spec_helper'

describe "workout_entries/edit" do
  before(:each) do
    @workout_entry = assign(:workout_entry, stub_model(WorkoutEntry))
  end

  it "renders the edit workout_entry form" do
    render

    # Run the generator again with the --webrat flag if you want to use webrat matchers
    assert_select "form", :action => workout_entries_path(@workout_entry), :method => "post" do
    end
  end
end
