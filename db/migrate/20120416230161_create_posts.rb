class CreatePosts < ActiveRecord::Migration
  def change
	remove_column :posts, :name
  end
 
  def down
	remove_column :name
  end
end
