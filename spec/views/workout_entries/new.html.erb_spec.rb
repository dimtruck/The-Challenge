require 'spec_helper'

describe "workout_entries/new" do
  before(:each) do
    assign(:workout_entry, stub_model(WorkoutEntry).as_new_record)
  end

  it "renders new workout_entry form" do
    render

    # Run the generator again with the --webrat flag if you want to use webrat matchers
    assert_select "form", :action => workout_entries_path, :method => "post" do
    end
  end
end
