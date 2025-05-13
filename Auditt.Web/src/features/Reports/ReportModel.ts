export interface ReportGlobalModel {
	countHistories: number;
	countHistoriesStrictAdherence: number;
	globalAdherence: number;
	strictAdherence: number;
}

export interface ReportQuestionAdherenceModel {
	countSuccess: number;
	countNoApply: number;
	countNoSuccess: number;
	valorationsCount: number;
	idQuestion?: number;
	text: string;
	percentSuccess: number;
}

export interface ReportModel {
	idReport: number;
	name: string;
	description: string;
}
