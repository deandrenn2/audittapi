export interface AssessmentCreateModel {
	idInstitucion: number;
	idDataCut: number;
	idFunctionary: number;
	idPatient: number;
	idGuide: number;
	yearOld?: string;
	date: Date;
	eps: string;
	idUser: number;
	id?: number;
}

export interface AssessmentListModel {
	id: number;
	identificationPatient: string;
	functionaryName: string;
	date: Date;
}

export interface AssessmentDetailModel {
	id: number;
	idDataCut: number;
	idFunctionary: number;
	idPatient: number;
	date: string;
	eps: string;
	idUserCreated: number;
	idUserUpdate: number;
	updateDate: string;
	createDate: string;
	valuations: ValuationModel[];
}

export interface ValuationModel {
	id: number;
	order: number;
	text: string;
	idAssessment: number;
	idEquivalence: number;
	idQuestion: number;
}
