require "spec_helper"

describe WorkoutEntriesController do
  describe "routing" do

    it "routes to #index" do
      get("/workout_entries").should route_to("workout_entries#index")
    end

    it "routes to #new" do
      get("/workout_entries/new").should route_to("workout_entries#new")
    end

    it "routes to #show" do
      get("/workout_entries/1").should route_to("workout_entries#show", :id => "1")
    end

    it "routes to #edit" do
      get("/workout_entries/1/edit").should route_to("workout_entries#edit", :id => "1")
    end

    it "routes to #create" do
      post("/workout_entries").should route_to("workout_entries#create")
    end

    it "routes to #update" do
      put("/workout_entries/1").should route_to("workout_entries#update", :id => "1")
    end

    it "routes to #destroy" do
      delete("/workout_entries/1").should route_to("workout_entries#destroy", :id => "1")
    end

  end
end
