class WorkoutEntriesController < ApplicationController
  # GET /workout_entries
  # GET /workout_entries.json
  def index
    @workout_entries = WorkoutEntry.all

    respond_to do |format|
      format.html # index.html.erb
      format.json { render json: @workout_entries }
    end
  end

  # GET /workout_entries/1
  # GET /workout_entries/1.json
  def show
    @workout_entry = WorkoutEntry.find(params[:id])

    respond_to do |format|
      format.html # show.html.erb
      format.json { render json: @workout_entry }
    end
  end

  # GET /workout_entries/new
  # GET /workout_entries/new.json
  def new
    @workout_entry = WorkoutEntry.new

    respond_to do |format|
      format.html # new.html.erb
      format.json { render json: @workout_entry }
    end
  end

  # GET /workout_entries/1/edit
  def edit
    @workout_entry = WorkoutEntry.find(params[:id])
  end

  # POST /workout_entries
  # POST /workout_entries.json
  def create
    @workout_entry = WorkoutEntry.new(params[:workout_entry])

    respond_to do |format|
      if @workout_entry.save
        format.html { redirect_to @workout_entry, notice: 'Workout entry was successfully created.' }
        format.json { render json: @workout_entry, status: :created, location: @workout_entry }
      else
        format.html { render action: "new" }
        format.json { render json: @workout_entry.errors, status: :unprocessable_entity }
      end
    end
  end

  # PUT /workout_entries/1
  # PUT /workout_entries/1.json
  def update
    @workout_entry = WorkoutEntry.find(params[:id])

    respond_to do |format|
      if @workout_entry.update_attributes(params[:workout_entry])
        format.html { redirect_to @workout_entry, notice: 'Workout entry was successfully updated.' }
        format.json { head :no_content }
      else
        format.html { render action: "edit" }
        format.json { render json: @workout_entry.errors, status: :unprocessable_entity }
      end
    end
  end

  # DELETE /workout_entries/1
  # DELETE /workout_entries/1.json
  def destroy
    @workout_entry = WorkoutEntry.find(params[:id])
    @workout_entry.destroy

    respond_to do |format|
      format.html { redirect_to workout_entries_url }
      format.json { head :no_content }
    end
  end
end
