require 'spec_helper'

describe Post do
	it "can be instantiated" do
		Post.new.should be_instance_of(Post)
	end
	
	it "validates that error is thrown when title does not exist" do
		post = Post.new(:content => '1234567890').should have(2).error_on(:title)
	end

	it "validates that error is thrown when title is less than 5 characters" do
		post = Post.new(:content => '123456790',:title => '1234').should have(1).error_on(:title)
	end
	
	it "validates that error is thrown when content does not exist" do
		post = Post.new(:title => '1234567890').should have(2).error_on(:content)
	end
	
	it "validates that error is thrown when content is less than 10 characters" do
		post = Post.new(:title => '1234567890', :content => 'noo!').should have(1).error_on(:content)
	end
	
	it "validates that error is thrown when type does not exist" do
		post = Post.new(:title => '1234567890').should have(2).error_on(:post_type)
	end
	
	it "validates that error is thrown when type is not nutrition or training" do
		post = Post.new(:title => '1234567890', :content => 'noo!', :post_type => 'nothing').should have(1).error_on(:post_type)
	end
	
	it "can be saved successfully with valid data" do
		post = Post.new(:title => 'test1', :content => 'this is enough', :post_type => 'diet')
		post.save
		Post.all.count.should == 1
		Post.all.each do |post|
			post.destroy
		end
	end
	
	it "should return a list of Posts" do
		Post.all.count.should == 0
		post = Post.new(:title => 'test1', :content => 'this is enough', :post_type => 'diet')
		post.save
		Post.all.count.should == 1
		
		Post.all.each do |post|
			post.destroy
		end
	end

	it "should return a list of Posts with type NUTRITION" do
		post = Post.new(:title => 'test1', :content => 'this is enough', :post_type => 'diet')
		post.save
		Post.all.count.should == 1
		Post.all.each do |post|
			post.post_type.should == 'diet'
		end
	end
	
	it "should return a list of Posts with type TRAINING" do
		post = Post.new(:title => 'test1', :content => 'this is enough', :post_type => 'training')
		post.save
		Post.all.count.should == 1
		Post.all.each do |post|
			post.post_type.should == 'training'
		end
	end
end
