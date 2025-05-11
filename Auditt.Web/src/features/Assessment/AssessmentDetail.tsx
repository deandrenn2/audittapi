import { faArrowCircleLeft } from "@fortawesome/free-solid-svg-icons"
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome"
import { Link } from "react-router-dom"
import { AssessmentCreate } from "./AssessmentCreate"
import { ClientSelect } from "../Clients/ClientSelect"
import { Option } from "../../shared/model";
import useUserContext from "../../shared/context/useUserContext"
export const AssessmentDetail = () => {
    const { client } = useUserContext();
    const selectedClient: Option | undefined = {
        value: client?.id?.toString(),
        label: client?.name,
    };

    return (
        <div className="w-full">
            <div className="flex gap-4 p-4 justify-between">
                <Link to={'/Assessments'}
                    title='Volver' className="bg-gray-300 hover:bg-gray-300 text-gray-700 hover:text-gray-800 border border-gray-400 hover:border-gray-600 px-4 py-2 rounded font-bold flex items-center transition-all">
                    <FontAwesomeIcon
                        icon={faArrowCircleLeft}
                        className="fa-search top-3 pr-2 font-bold"
                    />Volver
                </Link>

                <div className="flex items-center space-x-4 mb-4">
                    <span className="font-medium">IPS</span>
                    <ClientSelect className="w-lg" selectedValue={selectedClient} isSearchable={true} />
                </div>
                <Link to={'/Assessments/Create'} className="bg-[#392F5A] hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold mb-2" >
                    Ir a Indicadores e informes</Link>
            </div>
            <div className="bg-white p-4 rounded-lg">
                <AssessmentCreate />
            </div>
        </div>
    )
}
