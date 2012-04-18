class Post < ActiveRecord::Base
	validates :content, 	:presence => true,
							:length => { :minimum => 10 }
	validates :title, 		:presence => true,
							:length => { :minimum => 5 }
	validates :post_type,	:presence => true,
							:inclusion => { :in => %w(diet training)}
	attr_accessible :content, :title, :post_type
end
