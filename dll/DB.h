#pragma once
class DB
{
private:
	std::vector <Graph>* warehouse;
public:
	DB(std::vector <Graph>& v);
	DB(const DB& obj);
	DB& operator=(const DB& obj);
	void returnLog(std::ofstream& file);
	~DB();
};

