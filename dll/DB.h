#pragma once
class DB
{
private:
	std::vector <std::string>* warehouse;
public:
	DB(std::string answer);
	DB(const DB& obj);
	DB& operator=(const DB& obj);
	void returnLog(std::ofstream& file);
	~DB();
};

