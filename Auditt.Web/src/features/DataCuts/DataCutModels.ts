export interface DataCutModel {
	id: number;
	name: string;
	cycle?: string;
	initialDate: Date;
	finalDate: Date;
	maxHistory: number;
	institutionId: number;
}
